import ComplianceApi from '@/api/compliance.api'
import { getErrorMessage } from '@/helpers/error'

async function addUpdateCompliance(addUpdateCompliance) {
  let action = addUpdateCompliance.id > 0 ?
    ComplianceApi.compliance.update : ComplianceApi.compliance.add
  let message = 'Api error'
  let req = await action(addUpdateCompliance)
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function getComplianceById(id) {
  let req = await ComplianceApi.compliance.get(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}
 async function getComplianceList() {
  let req = await ComplianceApi.compliance.getByClient(1)

   //let req = await ComplianceApi.compliance.list({
   //  page: 1,
   //  limit: -1,
   //  search: '',
   //  sort: 'name asc'
   //})
  //let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}
async function deleteCompliance(id) {
  try {
    let message = 'Api error'

    let req = await ComplianceApi.compliance.delete(id)
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

export { addUpdateCompliance, getComplianceById, deleteCompliance, getComplianceList }
