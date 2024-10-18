public class StaticData
{
    public enum PuzzleEvent
    {
        START_TIMER,
        JOIN_TABLE,
        START_GAME,
        END_GAME,
        SUBMIT_TIMER,
        WINNER,
        ENTRYFEE,
        RES_TIMER
    }

    public static string baseURL = "https://api.kamaal19.com";
    //public static string baseURL = "https://eb06-110-226-125-131.ngrok-free.app";
    public static string userRegister = "/user/register";
    public static string userLogin = "/user/login";
    public static string getProfile = "/user/getProfile";
    public static string withdrawAmout = "/user/withdrawlAmount";
    public static string getNotification = "/user/getNotification";
    public static string getTermsAndCondition = "/user/getTermsAndCondition";
    public static string sendOTP = "/user/sendOtp";
    public static string verifyOtp = "/user/verifyOtp";
    public static string transactionHistory = "/user/getTransactionHistory";
    public static string getTopusers = "/user/getTopUsers";
    public static string userAccountView = "/user/getAccountOverview";
    public static string commisionPoints = "/user/commissionPoints";
}
