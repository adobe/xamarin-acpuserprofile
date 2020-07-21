/*
Copyright 2020 Adobe
All Rights Reserved.

NOTICE: Adobe permits you to use, modify, and distribute this file in
accordance with the terms of the Adobe license agreement accompanying
it. If you have received this file from a source other than Adobe,
then your use, modification, or distribution of it requires the prior
written permission of Adobe. (See LICENSE-MIT in the root folder for details)
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using Com.Adobe.Marketing.Mobile;
using System.Threading;

namespace ACPUserProfileTestApp.iOS
{
    public class ACPUserProfileExtensionService : IACPUserProfileExtensionService
    {
        TaskCompletionSource<string> stringOutput;
        private static CountdownEvent latch = null;
        private static string callbackString = "";

        public ACPUserProfileExtensionService()
        {
        }

        // ACPUserProfile methods
        public TaskCompletionSource<string> GetExtensionVersionUserProfile()
        {
            stringOutput = new TaskCompletionSource<string>();
            stringOutput.SetResult(ACPUserProfile.ExtensionVersion());
            return stringOutput;
        }

        public TaskCompletionSource<string> UpdateUserAttribute()
        {
            stringOutput = new TaskCompletionSource<string>();
            ACPUserProfile.UpdateUserAttribute("firstName", "john");
            stringOutput.SetResult("completed");
            GetUserAttributes();
            return stringOutput;
        }

        public TaskCompletionSource<string> UpdateUserAttributes()
        {
            stringOutput = new TaskCompletionSource<string>();
            var attributes = new NSMutableDictionary<NSString, NSString>
            {
                ["firstName"] = new NSString("jane"),
                ["lastName"] = new NSString("doe"),
                ["vehicle"] = new NSString("sedan"),
                ["color"] = new NSString("red"),
                ["insured"] = new NSString("true"),
                ["age"] = new NSString("21")
            };
            ACPUserProfile.UpdateUserAttributes(attributes);
            stringOutput.SetResult("complete");
            GetUserAttributes();
            return stringOutput;
        }

        public TaskCompletionSource<string> RemoveUserAttribute()
        {
            stringOutput = new TaskCompletionSource<string>();
            ACPUserProfile.RemoveUserAttribute("lastName");
            stringOutput.SetResult("completed");
            GetUserAttributes();
            return stringOutput;
        }

        public TaskCompletionSource<string> RemoveUserAttributes()
        {
            stringOutput = new TaskCompletionSource<string>();
            string[] attributes = new string[] { "vehicle", "color" };
            ACPUserProfile.RemoveUserAttributes(attributes);
            stringOutput.SetResult("completed");
            GetUserAttributes();
            return stringOutput;
        }

        public TaskCompletionSource<string> GetUserAttributes()
        {
            latch = new CountdownEvent(1);
            stringOutput = new TaskCompletionSource<string>();
            Action<NSDictionary, NSError> callback = (content, error) =>
             {
                 callbackString = "";
                 if (error != null)
                 {
                     string message = "GetUserAttributes error:" + error.DebugDescription;
                     Console.WriteLine(message);
                     callbackString = message;
                 }
                 else if (content == null)
                 {
                     string message = "GetUserAttributes callback is null.";
                     Console.WriteLine(message);
                     callbackString = message;
                 }
                 else
                 {
                     foreach (KeyValuePair<NSObject, NSObject> pair in content)
                     {
                         callbackString = callbackString + "[ " + pair.Key + ": " + pair.Value + " ]";
                     }
                     Console.WriteLine("Retrieved attributes: " + callbackString);
                 }
                 if (latch != null)
                 {
                     latch.Signal();
                 }
             };
            string[] attributes = new string[] { "firstName", "lastName", "vehicle", "color", "insured", "age" };
            ACPUserProfile.GetUserAttributes(attributes, callback);
            latch.Wait(1000);
            latch.Dispose();
            stringOutput.SetResult(callbackString);
            return stringOutput;
        }
    }

}