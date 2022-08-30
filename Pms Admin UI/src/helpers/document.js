import DocumentApi from '@/api/document.api'
import { getErrorMessage } from '@/helpers/error'

async function addUpdateDocument(addUpdateDocument) {
  let action = addUpdateDocument.id > 0 ?
    DocumentApi.document.update : DocumentApi.document.add
  let message = 'Api error'
  let req = await action(addUpdateDocument)
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function getDocumentById(id) {
  let req = await DocumentApi.document.get(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function deleteDocument(id) {
  try {
    let message = 'Api error'

    let req = await DocumentApi.document.delete(id)
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

export { addUpdateDocument, getDocumentById, deleteDocument }
