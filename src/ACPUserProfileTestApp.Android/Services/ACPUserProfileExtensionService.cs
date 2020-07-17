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
using Android.Runtime;
using Com.Adobe.Marketing.Mobile;
using System.Threading;
using Java.Util;
using Android.Telecom;
using Xamarin.Essentials;
using System.Linq;

namespace ACPUserProfileTestApp.Droid
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
            var attributes = new Dictionary<string, Java.Lang.Object>();
            attributes.Add("firstName", "jane");
            attributes.Add("lastName", "doe");
            attributes.Add("vehicle", "sedan");
            attributes.Add("color", "red");
            attributes.Add("insured", true);
            attributes.Add("age", 21);
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
            var attributes = new List<string>();
            attributes.Add("vehicle");
            attributes.Add("color");
            ACPUserProfile.RemoveUserAttributes(attributes);
            stringOutput.SetResult("completed");
            GetUserAttributes();
            return stringOutput;
        }

        public TaskCompletionSource<string> GetUserAttributes()
        {
            latch = new CountdownEvent(1);
            stringOutput = new TaskCompletionSource<string>();
            var attributes = new List<string>();
            attributes.Add("firstName");
            attributes.Add("lastName");
            attributes.Add("vehicle");
            attributes.Add("color");
            attributes.Add("insured");
            attributes.Add("age");
            ACPUserProfile.GetUserAttributes(attributes, new AdobeCallback());
            latch.Wait(1000);
            latch.Dispose();
            stringOutput.SetResult(callbackString);
            return stringOutput;
        }

        // callbacks
        class AdobeCallback : Java.Lang.Object, IAdobeCallbackWithError
        {
            public void Fail(AdobeError error)
            {
                callbackString = "";
                String message = "GetUserAttributes error:" + error.ToString();
                Console.WriteLine(message);
                callbackString = message;
            }

            public void Call(Java.Lang.Object retrievedAttributes)
            {
                callbackString = "";
                if (retrievedAttributes != null)
                {
                    var attributesDictionary = new Android.Runtime.JavaDictionary<string, object>(retrievedAttributes.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);
                    foreach (KeyValuePair<string, object> pair in attributesDictionary)
                    {
                        callbackString = callbackString + "[ " + pair.Key + ": " + pair.Value + " ]";
                        Console.WriteLine("Retrieved attributes: " + callbackString);
                    }
                }
                else
                {
                    callbackString = "GetUserAttributes callback is null.";
                }
                if (latch != null)
                {
                    latch.Signal();
                }
            }
        }
    }

}
