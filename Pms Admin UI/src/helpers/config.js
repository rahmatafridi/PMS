import ConfigApi from '@/api/config.api'
import { getErrorMessage } from '@/helpers/error'

async function addUpdateConfig(config) {
  let action = config.id > 0 ?
    ConfigApi.config.update : ConfigApi.config.add
  let message = 'Api error'
  let req = await action(config)
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}



async function getConfigById(id) {
  let req = await ConfigApi.config.get(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}


async function deleteConfig(id) {
  try {
    let message = 'Api error'

    let req = await ConfigApi.config.delete(id)
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



export {
  addUpdateConfig, getConfigById, deleteConfig
}
