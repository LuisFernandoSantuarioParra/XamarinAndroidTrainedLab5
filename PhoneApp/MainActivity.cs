using Android.App;
using Android.Widget;
using Android.OS;

namespace PhoneApp
{
    [Activity(Label = "PhoneApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        static readonly System.Collections.Generic.List<string> PhoneNumbers = new System.Collections.Generic.List<string>();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
           

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            var PhoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            var TraslateButton = FindViewById<Button>(Resource.Id.TraslateButton);
            var CallButton = FindViewById<Button>(Resource.Id.CallButton);
            var CallHistoryButton = FindViewById<Button>(Resource.Id.CallHistoryButton);
           

            CallButton.Enabled = false;
            var TraslatedNumber = string.Empty;

            TraslateButton.Click += (object sender, System.EventArgs e) =>
            {
                var Traslator = new PhoneTraslator();
                TraslatedNumber = Traslator.ToNumber(PhoneNumberText.Text);
                if (string.IsNullOrWhiteSpace(TraslatedNumber))
                {
                    //no hay numero al que llamar
                    CallButton.Text = "Llamar";
                    CallButton.Enabled = false;
                }
                else {
                    //hay un posible numero telefonico a llamar
                    CallButton.Text = $"Llamar al {TraslatedNumber}";
                    CallButton.Enabled = true;
                }

            };

            CallButton.Click += (object sender, System.EventArgs e)=>{
                //intentar marcar el número telefonico
                var CallDialog = new AlertDialog.Builder(this);
                CallDialog.SetMessage($"Llamar al número {TraslatedNumber}?");
                CallDialog.SetNeutralButton("LLamar",delegate {

                    //agregar el numero marcado a la lista de numeros marcados
                    PhoneNumbers.Add(TraslatedNumber);
                    //Habilitar el boton callhistorybutton
                    CallHistoryButton.Enabled = true;
                    //Crear un intento para marcar el número telefonico 
                    var CallIntent = new Android.Content.Intent(Android.Content.Intent.ActionCall);
                    CallIntent.SetData(Android.Net.Uri.Parse($"tel: {TraslatedNumber}"));
                    StartActivity(CallIntent);
                });
                CallDialog.SetNegativeButton("Cancelar",delegate { });
                //mostrar el cuadro de dialogo al usuario y esperar una respuesta 
                CallDialog.Show();
            };

            CallHistoryButton.Click += (sender, e) => {
                var Intent = new Android.Content.Intent(this,typeof(CallHistoryActivity));
                Intent.PutStringArrayListExtra("phone_numbers", PhoneNumbers);
                StartActivity(Intent);
                
                };

            Validate();

        }
        private async void Validate()
        {
            var menssage = FindViewById<TextView>(Resource.Id.message);
            
            var ServiceClient = new SALLab05.ServiceClient();

            string StudentEmail = "santuarioparral@hotmail.com";
            string Password = "santuario1";

            string myDevice = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            var Result = await ServiceClient.ValidateAsync(StudentEmail, Password, myDevice);
           
            menssage.Text=$"{Result.Status}\n{Result.Fullname}\n{Result.Token}";
          
        }


    }
}

