namespace FoodDeliveryApp.Models.AuthModels
{
    public class UserModel : BaseModel
    {
        private bool _isDriver;
        private bool _isOwner;
        private int _restaurantRefId;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _id = string.Empty;
        private string _loginToken;
        private string _fireBaseToken;
        private string _resetTokenPass;
        private string _newPassword;
        private string _telNo;

        public bool IsDriver { get => _isDriver; set => SetProperty(ref _isDriver, value); }
        public bool IsOwner { get => _isOwner; set => SetProperty(ref _isOwner, value); }
        public int CompanieRefId { get => _restaurantRefId; set => SetProperty(ref _restaurantRefId, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Id { get => _id; set => SetProperty(ref _id, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string LoginToken { get => _loginToken; set => SetProperty(ref _loginToken, value); }
        public string FireBaseToken { get => _fireBaseToken; set => SetProperty(ref _fireBaseToken, value); }
        public string TelNo { get => _telNo; set => SetProperty(ref _telNo, value); }
        public string ResetTokenPass { get => _resetTokenPass; set => SetProperty(ref _resetTokenPass, value); }
        public string NewPassword { get => _newPassword; set => SetProperty(ref _newPassword, value); }


    }
}
