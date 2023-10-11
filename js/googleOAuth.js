let googleAuthenticationStateProviderInstance = null;
export function GoogleAuthInitialize(clientId, googleAuthenticationStateProvider) {
    googleAuthenticationStateProviderInstance = googleAuthenticationStateProvider;
    google.accounts.id.initialize({ client_id: clientId, callback: blazorSchoolCallback });
}

function blazorSchoolCallback(googleResponse) {
    googleAuthenticationStateProviderInstance.invokeMethodAsync("GoogleLogin", { ClientId: googleResponse.clientId, SelectedBy: googleResponse.select_by, Credential: googleResponse.credential });
}