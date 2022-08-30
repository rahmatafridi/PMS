import PropertyApi from '@/api/property.api'
import { getErrorMessage } from '@/helpers/error'

async function addUpdateProperty(addUpdateProperty) {
    let action = addUpdateProperty.id > 0 ?
        PropertyApi.property.update : PropertyApi.property.add
    let message = 'Api error'
    let req = await action(addUpdateProperty)
    if (req.ok && req.data) {
        return Promise.resolve(req.data)
    } else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}

async function getPropertyById(id) {
    let req = await PropertyApi.property.get(id)
    let message = 'Api error'

    if (req.ok && req.data) {
        return Promise.resolve(req.data)
    }
    else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}

async function deleteProperty(id) {
    try {
        let message = 'Api error'

        let req = await PropertyApi.property.delete(id)
        if (req.ok && req.data) {
            return Promise.resolve(req.data)
        } else {
            message = getErrorMessage(req.error)
            return Promise.reject(message)
        }
    } catch (e) {
        return Promise.reject(e)
    } finally {
        //
    }
}

export { addUpdateProperty, getPropertyById, deleteProperty }
