namespace FoodDeliveryApp.Models.AuthModels
{
    public class UserModel : BaseModel
    {
        private bool _isDriver;
        private bool _isOwner;
        private int _restaurantRefId;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private double _coordX;
        private double _coordY;
        private string _id = string.Empty;
        public bool IsDriver { get => _isDriver; set => SetProperty(ref _isDriver, value); }
        public bool IsOwner { get => _isOwner; set => SetProperty(ref _isOwner, value); }
        public int RestaurantRefId { get => _restaurantRefId; set => SetProperty(ref _restaurantRefId, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Id { get => _id; set => SetProperty(ref _id, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public double CoordX { get => _coordX; set => SetProperty(ref _coordX, value); }
        public double CoordY { get => _coordY; set => SetProperty(ref _coordY, value); }
    }
}
