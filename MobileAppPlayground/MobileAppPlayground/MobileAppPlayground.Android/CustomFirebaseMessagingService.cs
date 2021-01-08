using System;
using System.Collections.Generic;
using System.Net.Http;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Util;
using AndroidX.Core.App;
using AndroidX.Work;
using Firebase.Messaging;
using MobileAppPlayground.Droid.WorkManager;

namespace MobileAppPlayground.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class CustomFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "CustomFirebaseMessagingService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);

            var notification = message.GetNotification();

            if (notification == null)
            {
                return;
            }

            var body = message.GetNotification().Body;
            Log.Debug(TAG, "Notification Message Body: " + body); 
            SendNotification(body, message.Data);
        }

        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);

            this.ScheduleTokenSync(p0);

            Log.Debug(TAG, "Refreshed token: " + p0);

            
        }

        private void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            var pendingIntent = PendingIntent.GetActivity(this,
                MainActivity.NOTIFICATION_ID,
                intent,
                PendingIntentFlags.OneShot);
            
            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.navigation_empty_icon)
                .SetContentTitle("FCM Message")
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }

        private void ScheduleTokenSync(string appToken)
        {
            var inputData = new AndroidX.Work.Data.Builder()
                .PutString(TokenSyncWorker.UserIdParamName, "some id")
                .PutString(TokenSyncWorker.AppTokenParamName, appToken)
                .Build();

            var tokenSyncWorkerRequest = new OneTimeWorkRequest.Builder(typeof(TokenSyncWorker))
                .SetConstraints(new Constraints.Builder()
                    .SetRequiredNetworkType(NetworkType.Connected)
                    .Build())
                .SetInputData(inputData)
                .AddTag(TokenSyncWorker.TAG)
                .Build();

            AndroidX.Work.WorkManager.GetInstance(this).BeginUniqueWork(
                    TokenSyncWorker.TAG, ExistingWorkPolicy.Replace, tokenSyncWorkerRequest)
                .Enqueue();
        }
    }
}