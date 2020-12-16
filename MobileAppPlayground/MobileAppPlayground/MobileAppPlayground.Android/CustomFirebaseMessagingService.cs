using System.Collections;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Util;
using AndroidX.Core.App;
using Firebase.Messaging;

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
            var body = message.GetNotification().Body;
            Log.Debug(TAG, "Notification Message Body: " + body); 
            SendNotification(body, message.Data);
        }

        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);

            Log.Debug(TAG, "Refreshed token: " + p0);

            // Send token to 360 api
        }

        void SendNotification(string messageBody, IDictionary<string, string> data)
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
    }
}