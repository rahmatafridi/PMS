import RoomApi from '@/api/room.api'
import { getErrorMessage } from '@/helpers/error'

async function addUpdateRoom(addUpdateRoom) {

  let action = addUpdateRoom.id > 0 ?
    RoomApi.room.update : RoomApi.room.add
  let message = 'Api error'
  let req = await action(addUpdateRoom)
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function getRoomById(id) {
  let req = await RoomApi.room.get(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function deleteRoom(id) {
  try {
    let message = 'Api error'

    let req = await RoomApi.room.delete(id)
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

export { addUpdateRoom, getRoomById, deleteRoom }
