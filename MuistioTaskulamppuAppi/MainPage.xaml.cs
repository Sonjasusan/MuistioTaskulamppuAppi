using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace MuistioTaskulamppuAppi
{
    public partial class MainPage : CarouselPage
    {
        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder
     .LocalApplicationData), "muistio.txt");

        string teksti = "";
        public MainPage()
        {
            InitializeComponent();
            bool doesExist = File.Exists(fileName); //Onko muistiinpanoja

            //Katsotaan onko olemassaolevia muistiinpanoja
            if (doesExist == true) //On olemassa
            {
                teksti = File.ReadAllText(fileName); //Luetaan tiedostot
                if (teksti.Length > 0)
                {
                    outputLabel.Text = teksti;
                }
                else
                {
                    outputLabel.Text = "Mitään ei ole talletettu muistioon.";
                }

            }
            else
            {
                outputLabel.Text = "Tervetuloa uudelle käyttäjälle!"; //Uusi käyttäjä, joka ei ole aiemmin tehnyt muistiinpanoja
            }
        }
        
        //MUISTION NAPIT

        private void tallennusNappi_Clicked(object sender, EventArgs e)
        {
            teksti = inputKentta.Text + Environment.NewLine + teksti; //Environment.NewLine = rivinvaihto (vastaa: "\n":ää)
            outputLabel.Text = teksti;
            inputKentta.Text = ""; //Tyhjennetään kirjoituskenttä
            File.WriteAllText(fileName, teksti); //Tallennetaan kirjoitettu teksti
        }

        private void poistoNappi_Clicked(object sender, EventArgs e)
        {
            vahvistusInfo.IsVisible = true; //Poistonappi esiin
            vahvistusKytkin.IsVisible = true;
            poistoNappi.IsVisible = false;
        }

        private void vahvistusKytkin_Toggled(object sender, ToggledEventArgs e) //Vahvistus muistiinpanojen tyhjennyksestä 
        {
            File.WriteAllText(fileName, ""); //Tyhjennetään
            teksti = "";
            outputLabel.Text = "Muistiinpanot tyhjennetty."; //Ilmoitetaan että poistettu
            poistoNappi.IsVisible = true; //Poistonappi tulee esiin
            vahvistusInfo.IsVisible = false; //Vahvistusinfo pois näkyvistä
            vahvistusKytkin.IsVisible=false; //Vahvistuskytkin pois näkyvistä
            vahvistusKytkin.IsToggled= false;
        }


        //TASKULAMPUN NAPIT

        async void valoNappi_Clicked(object sender, EventArgs e)
        {
            //Akun tason tarkistus
            var level = Battery.ChargeLevel;

            if (level < 0.05)//Jos n. 5%

            {
                Vibration.Vibrate(); //Puhelin värähtää
                await DisplayAlert("Akkuvirta vähissä", "Taskulamppua ei voida käyttää.", "ok"); //Ilmoitus
            }

            else //Muussa tapauksessa
            {
                await Flashlight.TurnOnAsync();
                valoNappi.IsVisible = false;
                valoPoisNappi.IsVisible = true;
            }            
        }

        async void valoPoisNappi_Clicked(object sender, EventArgs e)
        {
            await Flashlight.TurnOffAsync();
            valoNappi.IsVisible = true;
            valoPoisNappi.IsVisible = false;
        }
    }

}

