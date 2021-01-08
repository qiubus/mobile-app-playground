using Android.Content;
using Android.Util;
using AndroidX.Work;
using MobileAppPlayground.Droid.Services;

namespace MobileAppPlayground.Droid.WorkManager
{
    public class TokenSyncWorker : Worker
    {
        public const string TAG = "TokenSyncWorker";
        public const string UserIdParamName = "UserId";
        public const string AppTokenParamName = "AppTokenParamName";
        
        public TokenSyncWorker(Context context, WorkerParameters workerParams) :
            base(context, workerParams)
        {
        }

        public override Result DoWork()
        {
            Log.Debug(TAG, $"Start Sending token to API");

            var service = new MobileApiService();

            string userId = this.InputData.GetString(UserIdParamName);
            string appToken = this.InputData.GetString(AppTokenParamName);

            var isSuccess = service.SendAndroidTokenToApiAsync(userId, appToken).Result;

            if (isSuccess)
            {
                Log.Debug(TAG, $"End Sending token to API");

                return Result.InvokeSuccess();
            }

            return Result.InvokeRetry();
        }
    }
}