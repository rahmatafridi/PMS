import OptionApi from '@/api/option.api'
import { getErrorMessage } from '@/helpers/error'

async function addUpdateOption(addUpdateOption) {

  let action = addUpdateOption.id > 0 ?
    OptionApi.option.update : OptionApi.option.add
  let message = 'Api error'
  let req = await action(addUpdateOption)
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function getOptionById(id) {
  let req = await OptionApi.option.get(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}
async function getOptionListById(id) {
  let req = await OptionApi.option.getList(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}
async function getOptionByHeader(header) {
  let req = await OptionApi.option.getOptionByHeader(header)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}
async function deleteOption(id) {
  try {
    let message = 'Api error'

    let req = await OptionApi.option.delete(id)
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

async function validateOption(id,headerId,value,title) {
  let message = 'Api error'

  let req = await OptionApi.option.validateOption({
    id:id,
    headerId: headerId,
    value: value,
    title: title

  })
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}


export { addUpdateOption, getOptionById, deleteOption, getOptionListById, validateOption, getOptionByHeader }
