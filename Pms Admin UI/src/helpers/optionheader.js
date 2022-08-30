import OptionHeaderApi from '@/api/optionheader.api'
import { getErrorMessage } from '@/helpers/error'

async function addUpdateHeader(addUpdateHeader) {

  let action = addUpdateHeader.id > 0 ?
    OptionHeaderApi.header.update : OptionHeaderApi.header.add
  let message = 'Api error'
  let req = await action(addUpdateHeader)
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function getHeaderById(id) {
  let req = await OptionHeaderApi.header.get(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}
async function getHeaderListById(id) {
  let req = await OptionHeaderApi.header.getList(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function deleteHeader(id) {
  try {
    let message = 'Api error'

    let req = await OptionHeaderApi.header.delete(id)
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

export { addUpdateHeader, getHeaderById, deleteHeader, getHeaderListById }
