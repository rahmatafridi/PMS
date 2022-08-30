import RoomHistoryApi from '@/api/roomhistory.api'
import { getErrorMessage } from '@/helpers/error'



async function getRoomHistoryById(id) {
  let req = await RoomHistoryApi.room.get(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}



export { getRoomHistoryById}
