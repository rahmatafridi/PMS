import ProviderApi from '@/api/provider.api'
import { getErrorMessage } from '@/helpers/error'

async function addUpdateProvider(addUpdateprovider) {
    let action = addUpdateprovider.id > 0 ?
        ProviderApi.provider.update : ProviderApi.provider.add
    let message = 'Api error'
    let req = await action(addUpdateprovider)
    if (req.ok && req.data) {
        return Promise.resolve(req.data)
    } else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}

async function addProviderUser(addProvider) {
  let action = addProvider.id > 0 ?
    ProviderApi.provider.updateUser : ProviderApi.provider.adduser
  let message = 'Api error'
  let req = await action(addProvider)
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}


async function getProviderById(id) {
    let req = await ProviderApi.provider.get(id)
    let message = 'Api error'

    if (req.ok && req.data) {
        return Promise.resolve(req.data)
    }
    else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}
async function getProviderUserById(id) {
  let req = await ProviderApi.provider.getUser(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}


async function deleteProvider(id) {
    try {
        let message = 'Api error'

        let req = await ProviderApi.provider.delete(id)
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


async function validateProviderEmail(provider) {
    let message = 'Api error'
    let req = await ProviderApi.provider.validateEmail(provider.id, provider.email)
    if (req.ok && req.data) {
        return Promise.resolve(req.data)
    } else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}

async function getProviders(val, clientId) {
    let message = 'Api error'
    let req = await ProviderApi.provider.list({
        page: 1,
        limit: -1,
        search: val,
        sort: 'name asc',
        clientId: clientId
    })
    if (req.ok && req.data) {
        return Promise.resolve(req.data.items)
    } else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}

export {
    addUpdateProvider, getProviderById, deleteProvider, validateProviderEmail
  , getProviders, addProviderUser, getProviderUserById
}
