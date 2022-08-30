import NoteApi from '@/api/note.api'
import { getErrorMessage } from '@/helpers/error'

async function addUpdateNote(addUpdateNote) {
  let action = addUpdateNote.id > 0 ?
    NoteApi.note.update : NoteApi.note.add
  let message = 'Api error'
  let req = await action(addUpdateNote)
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function getNoteById(id,typeId) {
  let req = await NoteApi.note.get({
    noteId: id,
    typeId: typeId
  })
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function getNoteByParentId(id, typeId) {
  let req = await NoteApi.note.getParent({
    parentId: id,
    typeId: typeId
  })
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function deleteNote(id) {
  try {
    let message = 'Api error'

    let req = await NoteApi.note.delete(id)
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

export { addUpdateNote, getNoteById, deleteNote, getNoteByParentId }
