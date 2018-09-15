using System;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EmotionApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private CNTKGraphModel _model;
        private readonly string[] _emotions = { "neutral", "happiness", "surprise", "sadness", "anger", "disgust", "fear", "contempt" };
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
                new Uri("ms-appx:///FER-Emotion-Recognition.onnx"));
            _model = await CNTKGraphModel.CreateCNTKGraphModel(file);

            await Camera.StartAsync();
            Camera.CameraHelper.FrameArrived += CameraHelper_FrameArrived;
        }


        private async void CameraHelper_FrameArrived(object sender, Microsoft.Toolkit.Uwp.Helpers.FrameEventArgs e)
        {
            try
            {
                var input = new CNTKGraphModelInput() { Input338 = e.VideoFrame };
                var output = await _model.EvaluateAsync(input);
                var max = output.Plus692_Output_0.Max();
                var index = output.Plus692_Output_0.IndexOf(max);
                var text=$"{_emotions[index]}";
                Update(text);
            }
            catch (Exception exception)
            {
                //kill the exception
               
            }
            


        }

        private async void Update(string text)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Result.Text = text;
            });

            
        }
    }
}
