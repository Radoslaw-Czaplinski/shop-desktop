using NUnit.Framework;
using shop_desktop.Views;
using shop_desktop.Services;
using Moq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tests
{
    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class LoginWindowTests
    {
        private Mock<AuthenticationService> _authenticationServiceMock;
        private LoginWindow _loginWindow;

        [SetUp]
        public void Setup()
        {
            _authenticationServiceMock = new Mock<AuthenticationService>(null);
            _loginWindow = new LoginWindow(_authenticationServiceMock.Object);
        }

        [Test]
        public void LoginWindow_Constructor_ShouldInitializeComponent()
        {
            var window = new LoginWindow(_authenticationServiceMock.Object);
            Assert.IsNotNull(window);
        }

        [Test]
        public void LoginWindow_Constructor_ShouldSetAuthenticationService()
        {
            var window = new LoginWindow(_authenticationServiceMock.Object);
            Assert.AreEqual(_authenticationServiceMock.Object, window.GetType().GetField("_authenticationService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(window));
        }

        [Test]
        public void CancelButton_Click_ShouldCloseWindow()
        {
            _loginWindow.Show();
            var cancelButton = FindButtonByContent(_loginWindow, "Cancel");
            Assert.IsNotNull(cancelButton, "Button 'Cancel' should not be null.");

            cancelButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            Assert.IsFalse(_loginWindow.IsVisible);
        }

        private Button FindButtonByContent(DependencyObject parent, string content)
        {
            if (parent == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Button button && button.Content.ToString() == content)
                {
                    return button;
                }

                var result = FindButtonByContent(child, content);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}