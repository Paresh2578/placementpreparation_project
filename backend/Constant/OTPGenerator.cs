namespace backend.Constant
{
    public class OTPGenerator
    {
        public static int GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }
    }
}
