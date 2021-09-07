import appConfig from "../../appConfig";

class ApiRequestHeaders {
    constructor(authToken) {
        if (!authToken)
            this.headers = {
                ClientKey: appConfig.clientKey
            }
        else
            this.headers = { 
                AuthenticationToken: authToken,
                ClientKey: appConfig.clientKey
            }
    }
}

export default ApiRequestHeaders;