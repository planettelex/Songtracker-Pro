class ApiRequest {
    constructor(authToken) {
        this.headers = { AuthenticationToken: authToken }
    }
}

export default ApiRequest;