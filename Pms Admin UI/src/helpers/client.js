import ClientApi from '@/api/client.api'
import { getErrorMessage } from '@/helpers/error'

async function addUpdateClient(addUpdateClient) {
    let action = addUpdateClient.id > 0 ?
        ClientApi.client.update : ClientApi.client.add
    let message = 'Api error'
    let req = await action(addUpdateClient)
    if (req.ok && req.data) {
        return Promise.resolve(req.data)
    } else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}

async function getClientById(id) {
    let req = await ClientApi.client.get(id)
    let message = 'Api error'

    if (req.ok && req.data) {
        return Promise.resolve(req.data)
    }
    else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}

async function validateClientEmail(clientId, email) {

    let message = 'Api error'
  let req = await ClientApi.client.validateEmail(
    {
      clientId: clientId,
      email:email
    })
    if (req.ok && req.data) {
        return Promise.resolve(req.data)
    } else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}

async function deleteClient(id) {
    try {
        let message = 'Api error'

        let req = await ClientApi.client.delete(id)
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

async function getClients(val) {
    let message = 'Api error'
    let req = await ClientApi.client.list({
      page: 1,
      limit: -1,
      search: val,
      sort: 'name asc'
    })
    if (req.ok && req.data) {
      return Promise.resolve(req.data.items)
    } else {
      message = getErrorMessage(req.error)
      return Promise.reject(message)
    }
  }

export { addUpdateClient, getClientById, validateClientEmail, deleteClient
    , getClients }
