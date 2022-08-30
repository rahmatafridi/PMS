import ComplianceDocApi from '@/api/compliancedocs.api'
import { getErrorMessage } from '@/helpers/error'

async function addUpdateComplianceDoc(addUpdateComplianceDoc) {
  let action = addUpdateComplianceDoc.id > 0 ?
    ComplianceDocApi.compliance.update : ComplianceDocApi.compliance.add
  let message = 'Api error'
  let req = await action(addUpdateComplianceDoc)
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function getComplianceDocById(id) {
  let req = await ComplianceDocApi.compliance.get(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}
async function getCompliancePropDocById(id) {
  
  let req = await ComplianceDocApi.compliance.getPropDoc(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function deleteComplianceDoc(id) {
  try {
    let message = 'Api error'

    let req = await ComplianceDocApi.compliance.delete(id)
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

export { addUpdateComplianceDoc, getComplianceDocById, deleteComplianceDoc, getCompliancePropDocById}
