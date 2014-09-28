#region Usings

using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Runtime;

#endregion

namespace com.ashokgelal.segmentedcontrol
{
    public class SegmentedGroup : RadioGroup
    {
        #region Fields

        private Color _tintColor;
        private Color _checkedTextColor;
        private float _1dp;

        #endregion

        #region Constructors

        public SegmentedGroup(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            
        }

        public SegmentedGroup(Context context) : base(context)
        {
            Initialize();
        }

        public SegmentedGroup(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            _tintColor = Resources.GetColor(Resource.Color.radio_button_selected_color);
            _1dp = TypedValue.ApplyDimension(ComplexUnitType.Dip, 1, Resources.DisplayMetrics);
        }

        protected override void OnFinishInflate()
        {
            base.OnFinishInflate();
            UpdateBackground();
        }

        public void SetTintColor(Color tintColor)
        {
            _tintColor = tintColor;
            UpdateBackground();
        }

        public void SetColors(Color tintColor, Color checkedTextColor)
        {
            _tintColor = tintColor;
            _checkedTextColor = checkedTextColor;
            UpdateBackground();
        }

        public void SetCheckedTextColor(Color checkedTextColor)
        {
            _checkedTextColor = checkedTextColor;
            UpdateBackground();
        }

        private void UpdateBackground()
        {
            var count = ChildCount;
            if (count > 1)
            {
                var child = GetChildAt(0);
                var initParams = (LayoutParams)child.LayoutParameters;
                var newParams = new LayoutParams(initParams.Width, initParams.Height, initParams.Weight);
                newParams.SetMargins(0, 0, (int)(-_1dp), 0);
                child.LayoutParameters = newParams;
                UpdateBackground(GetChildAt(0), Resource.Drawable.radio_checked_left, Resource.Drawable.radio_unchecked_left);
                for (int i = 1; i < count - 1; i++)
                {
                    UpdateBackground(GetChildAt(i), Resource.Drawable.radio_checked_middle, Resource.Drawable.radio_unchecked_middle);
                    var child2 = GetChildAt(i);
                    initParams = (LayoutParams)child2.LayoutParameters;
                    newParams = new LayoutParams(initParams.Width, initParams.Height, initParams.Weight);
                    newParams.SetMargins(0, 0, (int)-_1dp, 0);
                    child2.LayoutParameters = newParams;
                }
                UpdateBackground(GetChildAt(count - 1), Resource.Drawable.radio_checked_right, Resource.Drawable.radio_unchecked_right);
            }
            else if (count == 1)
            {
                UpdateBackground(GetChildAt(0), Resource.Drawable.radio_checked_default, Resource.Drawable.radio_unchecked_default);
            }
        }

        private void UpdateBackground(View view, int hcecked, int nuchecked)
        {
            //Set text color
            var arr = new int[][]
            {
                new  []
                {
                    Android.Resource.Attribute.StatePressed
                },
                new []
                {
                    -Android.Resource.Attribute.StatePressed,
                    -Android.Resource.Attribute.StateChecked
                },
                new []
                {
                    -Android.Resource.Attribute.StatePressed,
                    Android.Resource.Attribute.StateChecked
                }
            };
            var colorStateList = new ColorStateList(
                                     arr,
                                     new int[]{ Color.Gray, _tintColor, _checkedTextColor });
            ((Button)view).SetTextColor(colorStateList);

            //Redraw with tint color
            var checkedDrawable = Resources.GetDrawable(hcecked).Mutate();
            var uncheckedDrawable = Resources.GetDrawable(nuchecked).Mutate();
            ((GradientDrawable)checkedDrawable).SetColor(_tintColor);
            ((GradientDrawable)uncheckedDrawable).SetStroke((int)_1dp, _tintColor);

            //Create drawable
            var stateListDrawable = new StateListDrawable();
            stateListDrawable.AddState(new []{ -Android.Resource.Attribute.StateChecked }, uncheckedDrawable);
            stateListDrawable.AddState(new []{ Android.Resource.Attribute.StateChecked }, checkedDrawable);

            //Set button background
            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
            {
                view.Background = stateListDrawable;
            }
            else
            {
                view.SetBackgroundDrawable(stateListDrawable);
            }
        }

        #endregion
    }
}

