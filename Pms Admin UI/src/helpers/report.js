import ReportApi from '@/api/report.api'
import { getErrorMessage } from '@/helpers/error'


async function getTenantReport(typeId) {
  let req = await ReportApi.report.tenantReport(typeId)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}
async function getEmptyRoomReport(clientId) {

  let req = await ReportApi.report.tenantReport(typeId)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}



export { getTenantReport }
