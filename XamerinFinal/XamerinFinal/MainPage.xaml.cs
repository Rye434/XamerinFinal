using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamerinFinal
{
	public partial class MainPage : ContentPage
	{
        const string APIKEY = "7a7bccecb620420089ae8e1ed3d7104f";
        const string APIROOT = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0";

        public MainPage()
		{
			InitializeComponent();
		}

        private async Task<AnalysisResult> GetImageDescription(Stream imageStream)
        {
            VisionServiceClient visionClient = new VisionServiceClient(APIKEY, APIROOT);
            List<VisualFeature> features = new List<VisualFeature> { VisualFeature.Tags, VisualFeature.Description };

            return await visionClient.AnalyzeImageAsync(imageStream, features, null);
        }

        private void ProcessImageResults(AnalysisResult result)
        {
            theResults.Text = "";

            theResults.Text = "";
        }

        private async void GetImageDataFromFile(MediaFile file)
        {
            theActivityIndicator.IsRunning = true;
            try
            {
                var result = await GetImageDescription(file.GetStream());

                ProcessImageResults(result);

            }
            catch (ClientException ex)
            {
                theResults.Text = ex.Message;
            }
            theActivityIndicator.IsRunning = false;
        }

        private async void Speak()
        {
            // need to add code here!
        }

        private async void WebImageButton_Clicked(object sender, EventArgs e)
        {
            Uri webImageUri = new Uri("https://placeimg.com/640/480");
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using(var response = await client.GetStreamAsync(webImageUri))
                    {
                        theActivityIndicator.IsRunning = true;
                        var memoryStream = new MemoryStream();
                        await response.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        

                        try
                        {
                            var result = await GetImageDescription(memoryStream);
                            ProcessImageResults(result);
                            memoryStream.Position = 0;
                            theImage.Source = ImageSource.FromStream(() => memoryStream);
                        }
                        catch (ClientException ex)
                        {
                            theResults.Text = ex.Message;
                        }

                        theActivityIndicator.IsRunning = false;
                    }
                }
            }
            catch (Exception ex)
            {
                theResults.Text = "Failed to load the image: " + ex.Message;
            }
        }
        private async void CameraButton_Clicked(object sender, EventArgs e)
        {

        }
        private async void ImageButton_Clicked(object sender, EventArgs e)
        {

        }

    }
}
