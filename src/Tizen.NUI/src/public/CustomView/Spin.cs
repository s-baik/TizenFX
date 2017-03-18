// Copyright (c) 2017 Samsung Electronics Co., Ltd.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
//
// This File has been auto-generated by SWIG and then modified using DALi Ruby Scripts
// Some have been manually changed

using System;
using System.Runtime.InteropServices;
using Tizen.NUI;

// A spin control (for continously changing values when users can easily predict a set of values)

namespace Tizen.NUI
{
    ///<summary>
    ///Spin CustomView class
    /// </summary>
    public class Spin : CustomView
    {
        private VisualBase _arrowVisual;
        private TextField _textField;
        private int _arrowVisualPropertyIndex;
        private string _arrowImage;
        private int _currentValue;
        private int _minValue;
        private int _maxValue;
        private int _singleStep;
        private bool _wrappingEnabled;
        private string _fontFamily;
        private string _fontStyle;
        private int _pointSize;
        private Color _textColor;
        private Color _textBackgroundColor;
        private int _maxTextLength;

        // Called by DALi Builder if it finds a Spin control in a JSON file
        static CustomView CreateInstance()
        {
            return new Spin();
        }

        // static constructor registers the control type (only runs once)
        static Spin()
        {
            // ViewRegistry registers control type with DALi type registery
            // also uses introspection to find any properties that need to be registered with type registry
            ViewRegistry.Instance.Register(CreateInstance, typeof(Spin));
        }

        public Spin() : base(typeof(Spin).Name, CustomViewBehaviour.RequiresKeyboardNavigationSupport)
        {

        }

        /// <summary>
        /// Override method of OnInitialize() for CustomView class.
        /// This method is called after the Control has been initialized.
        /// Derived classes should do any second phase initialization by overriding this method.
        /// </summary>
        public override void OnInitialize()
        {
            // Initialize the propertiesControl
            _arrowImage = "/home/owner/apps_rw/NUISamples.TizenTV/res/images/arrow.png";
            _textBackgroundColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
            _currentValue = 0;
            _minValue = 0;
            _maxValue = 0;
            _singleStep = 1;
            _maxTextLength = 0;

            // Create image visual for the arrow keys
            _arrowVisualPropertyIndex = RegisterProperty("ArrowImage", new PropertyValue(_arrowImage), Tizen.NUI.PropertyAccessMode.ReadWrite);
            _arrowVisual = VisualFactory.Get().CreateVisual(_arrowImage, new Uint16Pair(150, 150));
            RegisterVisual(_arrowVisualPropertyIndex, _arrowVisual);

            // Create a text field
            _textField = new TextField();
            _textField.ParentOrigin = Tizen.NUI.ParentOrigin.Center;
            _textField.AnchorPoint = Tizen.NUI.AnchorPoint.Center;
            _textField.WidthResizePolicy = ResizePolicyType.SizeRelativeToParent;
            _textField.HeightResizePolicy = ResizePolicyType.SizeRelativeToParent;
            _textField.SizeModeFactor = new Vector3(1.0f, 0.45f, 1.0f);
            _textField.PlaceholderText = "----";
            _textField.BackgroundColor = _textBackgroundColor;
            _textField.HorizontalAlignment = "Center";
            _textField.VerticalAlignment = "Center";
            _textField.Focusable = (true);
            _textField.Name = "_textField";

            this.Add(_textField);

            _textField.FocusGained += TextFieldKeyInputFocusGained;
            _textField.FocusLost += TextFieldKeyInputFocusLost;
        }

        /// <summary>
        /// Override method of GetNaturalSize() for CustomView class.
        /// Return the natural size of the actor.
        /// </summary>
        /// <returns> Natural size of this Spin itself</returns>
        public override Size GetNaturalSize()
        {
            return new Size(150.0f, 150.0f, 0.0f);
        }

        /// <summary>
        /// Event handler when the TextField in Spin gets the Key focus
        /// Make sure when the current spin that takes input focus also takes the keyboard focus
        /// For example, when you tap the spin directly
        /// </summary>
        /// <param name="source">Sender of this event</param>
        /// <param name="e">Event arguments</param>
        public void TextFieldKeyInputFocusGained(object source, EventArgs e)
        {
            FocusManager.Instance.SetCurrentFocusView(_textField);
        }

        /// <summary>
        /// Event handler when the TextField in Spin looses it's Key focus
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void TextFieldKeyInputFocusLost(object source, EventArgs e)
        {
            int previousValue = _currentValue;

            // If the input value is invalid, change it back to the previous valid value
            if (int.TryParse(_textField.Text, out _currentValue))
            {
                if (_currentValue < _minValue || _currentValue > _maxValue)
                {
                    _currentValue = previousValue;
                }
            }
            else
            {
                _currentValue = previousValue;
            }

            // Otherwise take the new value
            this.Value = _currentValue;
        }

        /// <summary>
        /// Override method of GetNextKeyboardFocusableView() for CustomView class.
        /// Gets the next key focusable view in this View towards the given direction.
        /// A View needs to override this function in order to support two dimensional key navigation.
        /// </summary>
        /// <param name="currentFocusedView">The current focused view</param>
        /// <param name="direction">The direction to move the focus towards</param>
        /// <param name="loopEnabled">Whether the focus movement should be looped within the control</param>
        /// <returns>The next keyboard focusable view in this control or an empty handle if no view can be focused</returns>
        public override View GetNextKeyboardFocusableView(View currentFocusedView, View.FocusDirection direction, bool loopEnabled)
        {
            // Respond to Up/Down keys to change the value while keeping the current spin focused
            View nextFocusedView = currentFocusedView;
            if (direction == View.FocusDirection.Up)
            {
                this.Value += this.Step;
                nextFocusedView = _textField;
            }
            else if (direction == View.FocusDirection.Down)
            {
                this.Value -= this.Step;
                nextFocusedView = _textField;
            }
            else
            {
                // Return a native empty handle as nothing can be focused in the left or right
                nextFocusedView = new View();
                nextFocusedView.Reset();
            }

            return nextFocusedView;
        }

        /// <summary>
        /// Value to be set in Spin
        /// </summary>
        [ScriptableProperty()]
        public int Value
        {
            get
            {
                return _currentValue;
            }
            set
            {

                Console.WriteLine("Value set to " + value);
                _currentValue = value;

                // Make sure no invalid value is accepted
                if (_currentValue < _minValue)
                {
                    _currentValue = _minValue;
                }

                if (_currentValue > _maxValue)
                {
                    _currentValue = _maxValue;
                }

                _textField.Text = _currentValue.ToString();
            }
        }
        
        /// <summary>
        /// Minimum Value of Spin Value
        /// </summary>
        // MinValue property of type int:
        [ScriptableProperty()]
        public int MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;
            }
        }

        /// <summary>
        /// Maximum Value of Spin Value
        /// </summary>
        // MaxValue property of type int:
        [ScriptableProperty()]
        public int MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
            }
        }

        /// <summary>
        ///  Increasing, decresing step of Spin Value when Up or Down key is pressed
        /// </summary>
        // Step property of type int:
        [ScriptableProperty()]
        public int Step
        {
            get
            {
                return _singleStep;
            }
            set
            {
                _singleStep = value;
            }
        }

        /// <summary>
        /// Wrapping enabled status
        /// </summary>
        // WrappingEnabled property of type bool:
        [ScriptableProperty()]
        public bool WrappingEnabled
        {
            get
            {
                return _wrappingEnabled;
            }
            set
            {
                _wrappingEnabled = value;
            }
        }

        /// <summary>
        ///  Text point size of Spin Value
        /// </summary>
        // TextPointSize property of type int:
        [ScriptableProperty()]
        public int TextPointSize
        {
            get
            {
                return _pointSize;
            }
            set
            {
                _pointSize = value;
                _textField.PointSize = _pointSize;
            }
        }

        /// <summary>
        /// The color of Spin Value
        /// </summary>
        // TextColor property of type Color:
        [ScriptableProperty()]
        public Color TextColor
        {
            get
            {
                return _textColor;
            }
            set
            {
                Console.WriteLine("TextColor set to " + value.R + "," + value.G + "," + value.B);

                _textColor = value;
                _textField.TextColor = _textColor;
            }
        }

        /// <summary>
        /// Maximum text lengh of Spin Value
        /// </summary>
        // MaxTextLength property of type int:
        [ScriptableProperty()]
        public int MaxTextLength
        {
            get
            {
                return _maxTextLength;
            }
            set
            {
                _maxTextLength = value;
                _textField.MaxLength = _maxTextLength;
            }
        }

        /// <summary>
        /// Reference of TextField of Spin
        /// </summary>
        public TextField SpinText
        {
            get
            {
                return _textField;
            }
            set
            {
                _textField = value;
            }
        }

        /// <summary>
        /// Show indicator image, for example Up/Down Arrow image.
        /// </summary>
        // Indicator property of type string:
        public string IndicatorImage
        {
            get
            {
                return _arrowImage;
            }
            set
            {
                _arrowImage = value;
                _arrowVisual = VisualFactory.Get().CreateVisual(_arrowImage, new Uint16Pair(150, 150));
                RegisterVisual(_arrowVisualPropertyIndex, _arrowVisual);
            }
        }
    }
}