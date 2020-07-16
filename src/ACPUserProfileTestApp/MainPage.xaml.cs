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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ACPUserProfileTestApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // ACPUserProfile API
        void OnExtensionVersionButtonClicked(object sender, EventArgs args)
        {
            string result = DependencyService.Get<IACPUserProfileExtensionService>().GetExtensionVersionUserProfile().Task.Result;
            handleStringResult("GetExtensionVersionUserProfile", result);
        }

        void OnUpdateUserAttributeButtonClicked(object sender, EventArgs args)
        {
            string result = DependencyService.Get<IACPUserProfileExtensionService>().UpdateUserAttribute().Task.Result;
            handleStringResult("UpdateUserAttribute", result);
        }

        void OnUpdateUserAttributesButtonClicked(object sender, EventArgs args)
        {
            string result = DependencyService.Get<IACPUserProfileExtensionService>().UpdateUserAttributes().Task.Result;
            handleStringResult("UpdateUserAttributes", result);
        }

        void OnRemoveUserAttributeButtonClicked(object sender, EventArgs args)
        {
            string result = DependencyService.Get<IACPUserProfileExtensionService>().RemoveUserAttribute().Task.Result;
            handleStringResult("RemoveUserAttribute", result);
        }

        void OnRemoveUserAttributesButtonClicked(object sender, EventArgs args)
        {
            string result = DependencyService.Get<IACPUserProfileExtensionService>().RemoveUserAttributes().Task.Result;
            handleStringResult("RemoveUserAttributes", result);
        }

        void OnGetUserAttributesButtonClicked(object sender, EventArgs args)
        {
            string result = DependencyService.Get<IACPUserProfileExtensionService>().GetUserAttributes().Task.Result;
            handleStringResult("GetUserAttributes", result);
        }

        // helper methods
        private void handleStringResult(string apiName, string result)
        {
            if (result != null)
            {
                Console.WriteLine(apiName + ": " + result);
            }
        }
    }
}
