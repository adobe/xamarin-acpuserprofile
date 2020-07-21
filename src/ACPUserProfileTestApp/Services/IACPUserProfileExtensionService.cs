/*
Copyright 2020 Adobe
All Rights Reserved.

NOTICE: Adobe permits you to use, modify, and distribute this file in
accordance with the terms of the Adobe license agreement accompanying
it. If you have received this file from a source other than Adobe,
then your use, modification, or distribution of it requires the prior
written permission of Adobe. (See LICENSE-MIT for details)
*/

using System;
using System.Threading.Tasks;

namespace ACPUserProfileTestApp
{
    public interface IACPUserProfileExtensionService
    {
        // ACPUserProfile API
        TaskCompletionSource<string> GetExtensionVersionUserProfile();
        TaskCompletionSource<string> UpdateUserAttribute();
        TaskCompletionSource<string> UpdateUserAttributes();
        TaskCompletionSource<string> RemoveUserAttribute();
        TaskCompletionSource<string> RemoveUserAttributes();
        TaskCompletionSource<string> GetUserAttributes();
    }
}
