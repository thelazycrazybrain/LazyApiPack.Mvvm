using LazyApiPack.Wpf.Utils.Converters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace LazyApiPack.Mvvm.Wpf.Tests
{
    [Apartment(ApartmentState.STA)]
    public class BoolToVisibilityConverterTests
    {
        BoolToVisibilityConverter _boolToVisibilityConverter;
        [SetUp]
        public void Setup()
        {
            _boolToVisibilityConverter = new BoolToVisibilityConverter();
        }

        [Test]
        public void BoolToVisibilityTest()
        {
            var uc1 = new Foo1();
           
            SetBoolBinding(uc1, "!");
            uc1.IsFooVisible = false;
            Assert.That(uc1.Visibility, Is.EqualTo(Visibility.Visible));

            uc1.Visibility = Visibility.Collapsed;
            Assert.That(uc1.IsFooVisible, Is.EqualTo(true));
            BindingOperations.ClearBinding(uc1, UIElement.VisibilityProperty);


            // Same behavior as previous
            SetBoolBinding(uc1, "invert");
            uc1.IsFooVisible = false;
            Assert.That(uc1.Visibility, Is.EqualTo(Visibility.Visible));

            uc1.Visibility = Visibility.Collapsed;
            Assert.That(uc1.IsFooVisible, Is.EqualTo(true));
            BindingOperations.ClearBinding(uc1, UIElement.VisibilityProperty);

            // Hidden, not inverted
            SetBoolBinding(uc1, "hidden");
            uc1.IsFooVisible = false;
            Assert.That(uc1.Visibility, Is.EqualTo(Visibility.Hidden));

            uc1.Visibility = Visibility.Collapsed;
            Assert.That(uc1.IsFooVisible, Is.EqualTo(false));
            BindingOperations.ClearBinding(uc1, UIElement.VisibilityProperty);

            // Hidden, inverted
            SetBoolBinding(uc1, "!;hidden");
            uc1.IsFooVisible = false;
            Assert.That(uc1.Visibility, Is.EqualTo(Visibility.Visible));

            uc1.Visibility = Visibility.Collapsed;
            Assert.That(uc1.IsFooVisible, Is.EqualTo(true));
            BindingOperations.ClearBinding(uc1, UIElement.VisibilityProperty);

            // No parameter
            SetBoolBinding(uc1, null);
            uc1.IsFooVisible = false;
            Assert.That(uc1.Visibility, Is.EqualTo(Visibility.Collapsed));

            uc1.Visibility = Visibility.Visible;
            Assert.That(uc1.IsFooVisible, Is.EqualTo(true));
            BindingOperations.ClearBinding(uc1, UIElement.VisibilityProperty);

            Assert.Pass();

        }

        private void SetBoolBinding(FrameworkElement dp, string? parameter)
        {

            BindingOperations.SetBinding(dp, Foo1.VisibilityProperty, new Binding()
            {
                Path = new PropertyPath("IsFooVisible", null),
                Source = dp,
                Converter =_boolToVisibilityConverter,
                ConverterParameter = parameter,
                Mode = BindingMode.TwoWay
            });

        }
    }



    public class Foo1 : UserControl
    {
        public Foo1()
        {
            Background = Brushes.Red;
        }

        public static readonly DependencyProperty IsFooVisibleProperty = DependencyProperty.Register(nameof(IsFooVisible), typeof(bool), typeof(Foo1), new PropertyMetadata(true));
        public bool IsFooVisible
        {
            get { return (bool)GetValue(IsFooVisibleProperty); }
            set { SetValue(IsFooVisibleProperty, value); }
        }

    }

}
