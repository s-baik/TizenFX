﻿/*
* Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
*
* Licensed under the Apache License, Version 2.0 (the License);
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an AS IS BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/


using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tizen.Content.MediaContent
{
    /// <summary>
    /// A Media Group represents logical grouping of media files with respect to their group name.
    /// It is also used for filtering media items.
    /// </summary>
    public class Group : ContentCollection
    {
        private readonly string _groupName;
        private readonly MediaGroupType _groupType;
        private bool _disposedValue = false;

        /// <summary>
        /// The name of the media group
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public string Name
        {
            get { return _groupName; }
        }

        internal Group(string name, MediaGroupType groupType)
        {
            _groupName = name;
            _groupType = groupType;
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposedValue = true;
            }
        }

        /// <summary>
        /// Gets the count of the media info for the given media group present in the media database.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <param name="filter">ContentFilter used to match media content from the media database.</param>
        /// <returns>The number of media contents matching the filter passed</returns>
        public override int GetMediaInformationCount(ContentFilter filter)
        {
            int mediaCount = 0;
            IntPtr handle = (filter != null) ? filter.Handle : IntPtr.Zero;
            MediaContentValidator.ThrowIfError(
                Interop.Group.GetMediaCountFromDb(Name, _groupType, handle, out mediaCount), "Failed to GetMediaCountFromDb");

            return mediaCount;
        }


        /// <summary>
        /// Iterates through the media files with an optional filter in the given group from the media database.
        /// This function gets all media files associated with the given group and meeting desired filter option.
        /// If NULL is passed to the filter, no filtering is applied.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <param name="filter">ContentFilter used to match media content from the media database.</param>
        /// <returns>List of content media items matching the passed filter</returns>
        public override IEnumerable<MediaInformation> GetMediaInformations(ContentFilter filter)
        {
            List<MediaInformation> mediaContents = new List<MediaInformation>();
            IntPtr handle = (filter != null) ? filter.Handle : IntPtr.Zero;
            Interop.Group.MediaInfoCallback callback = (IntPtr mediaHandle, IntPtr data) =>
            {
                Interop.MediaInformation.SafeMediaInformationHandle newHandle;
                MediaContentValidator.ThrowIfError(
                    Interop.MediaInformation.Clone(out newHandle, mediaHandle), "Failed to clone MediaInformation instance");

                MediaContentType type;
                Interop.MediaInformation.GetMediaType(newHandle, out type);
                if (type == MediaContentType.Image)
                {
                    Interop.ImageInformation.SafeImageInformationHandle imageInfo;
                    MediaContentValidator.ThrowIfError(
                        Interop.MediaInformation.GetImage(mediaHandle, out imageInfo), "Failed to get image information");

                    mediaContents.Add(new ImageInformation(imageInfo, newHandle));
                }
                else if ((type == MediaContentType.Music) || (type == MediaContentType.Sound))
                {
                    Interop.AudioInformation.SafeAudioInformationHandle audioInfo;
                    MediaContentValidator.ThrowIfError(
                        Interop.MediaInformation.GetAudio(mediaHandle, out audioInfo), "Failed to get audio information");

                    mediaContents.Add(new AudioInformation(audioInfo, newHandle));
                }
                else if (type == MediaContentType.Video)
                {
                    Interop.VideoInformation.SafeVideoInformationHandle videoInfo;
                    MediaContentValidator.ThrowIfError(
                        Interop.MediaInformation.GetVideo(mediaHandle, out videoInfo), "Failed to get video information");

                    mediaContents.Add(new VideoInformation(videoInfo, newHandle));
                }
                else if (type == MediaContentType.Others)
                {
                    mediaContents.Add(new MediaInformation(newHandle));
                }

                return true;
            };
            MediaContentValidator.ThrowIfError(
                Interop.Group.ForeachMediaFromDb(Name, _groupType, handle, callback, IntPtr.Zero), "Failed to get media information for the group");

            return mediaContents;
        }
    }
}