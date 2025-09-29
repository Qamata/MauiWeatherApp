namespace MauiWeatherApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnSearchCompleted(object sender, EventArgs e)
        {
            var viewModel = BindingContext as ViewModels.WeatherViewModel;
            var entry = sender as Entry;

            if (viewModel != null && !string.IsNullOrWhiteSpace(entry.Text))
            {
                viewModel.SearchCommand.Execute(entry.Text);
            }
        }
    }
}
