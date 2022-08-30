function getErrorMessage(error) {
    let message = null
    if (error.response) {
        if (error && error.response && error.response.data) {
            message = error.response.data.message
        }
    }
    else if (error) {
        message = error.message
    }
    return message
}

export { getErrorMessage }