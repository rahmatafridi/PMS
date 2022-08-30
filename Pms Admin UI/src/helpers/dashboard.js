import DashbaordApi from '@/api/dashboard.api'
import { getErrorMessage } from '@/helpers/error'
import store from '@/store/index'
async function getDataa() {
  var clientId = store.state.login.user.clientId;
  let req = await DashbaordApi.dashboard.loadData(clientId);
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}





export { getDataa}
