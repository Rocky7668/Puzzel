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
    public static string sendOTP = "/user/auth/sendOtp";
    public static string verifyOtp = "/user/auth/verifyOtp";
    public static string transactionHistory = "/user/getTransactionHistory";
    public static string getTopusers = "/user/getTopUsers";
    public static string userAccountView = "/user/getAccountOverview";
    public static string commisionPoints = "/user/commissionPoints";
    public static string depositAmount = "/user/depositAmount";
    public static string userWithdrawHistory = "/user/getWithdrawHistory";
    public static string userDepositeHistory = "/user/getDepositHistory";
    public static string GetImage = "/user/getGamePlayImage";
    public static string AddBank = "/user/addBank";
    public static string UpdateBank = "/user/updateBank";
    public static string GetBankHistory = "/user/getBankHistory";
    public static string GetPaymentDetails = "/user/getPaymentDetails";
    public static string GetRefundUserList = "/user/getRefundUsersList";
    public static string AddUPI = "/user/addUpi";
    public static string UpdateUPI = "/user/updateUpi";
    public static string GetUPIHistory = "/user/upiHistory";
    public static string GetCommissionHistory = "/user/getCommissionHistory";
    public static string UpdateProfile = "/user/updateProfile";
    public static string UpdateUsername = "/user/updateUserName";
    public static string UpdateEmail = "/user/updateEmail";
    public static string getPrivacyPolicy = "/user/getPrivacyAndPolicy";
    public static string getGstPolicy = "/user/getGSTPolicy";
    public static string getLegality = "/user/getLegality";
    public static string SendOtpUsername = "/user/sendOtp";
    public static string VerifyOtpUsername = "/user/verifyOtp";
    public static string GetGameHistory = "/user/getGameHistory";


    public static double TotalBalance;
    public static double TotalCommission;
}
