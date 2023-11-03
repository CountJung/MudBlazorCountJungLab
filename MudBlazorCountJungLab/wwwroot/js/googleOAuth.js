let googleAuthenticationStateProviderInstance = null;
export function GoogleAuthInitialize(clientId, googleAuthenticationStateProvider) {
    googleAuthenticationStateProviderInstance = googleAuthenticationStateProvider;
    google.accounts.id.initialize({ client_id: clientId, callback: blazorSchoolCallback });
}

export function blazorSchoolGooglePrompt() {
    google.accounts.id.prompt((notification) => {
        if (notification.isNotDisplayed() || notification.isSkippedMoment()) {
            console.info(notification.getNotDisplayedReason());
            console.info(notification.getSkippedReason());
        }
    });
}

function blazorSchoolCallback(googleResponse) {
    googleAuthenticationStateProviderInstance.invokeMethodAsync("GoogleLogin", { ClientId: googleResponse.clientId, SelectedBy: googleResponse.select_by, Credential: googleResponse.credential });
}
