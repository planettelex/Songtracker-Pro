class ErrorHandler {
    constructor(error) {
        this.error = error;
        this.is401 = error.message.includes("status code 401");
    }

    handleError(router) {
        if (this.is401) router.push("/login?logout=true&reason=expired");
        return this.error;
    }
}

export default ErrorHandler;