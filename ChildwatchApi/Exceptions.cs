using System;

namespace ChildWatchApi
{
    public class InvalidLoginException : Exception
    {
        private const string mainMessage = "Unable to validate member with credentials provided";
        private string pin;
        private string barcode;
        public bool PinInvalid
        {
            get { return pin.Length != 4; }
        }
        public bool BarcodeInvalid
        {
            get { return barcode.Length != 6; }
        }

        public InvalidLoginException(string barcode, string pin) : base(mainMessage + " barcode: " + barcode + "  pin: " + pin) 
        {
            this.barcode = barcode;
            this.pin = pin;
        }

        
    }

}
