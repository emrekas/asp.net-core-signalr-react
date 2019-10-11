using ChatSample.Entities;

namespace ChatSample.IServices {
    public interface IUserService {
        User Authenticate (string username, string password);
    }
}