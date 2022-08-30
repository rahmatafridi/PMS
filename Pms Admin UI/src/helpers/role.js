import RoleApi from '@/api/role.api'
import { getErrorMessage } from '@/helpers/error'
import store from '@/store/index'

async function addUpdateRole(addUpdateRole) {
  let message = 'Api error'
  let action = addUpdateRole.id > 0 ? RoleApi.role.update : RoleApi.role.add
  const req = await action(addUpdateRole)
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function getRoleById(id) {
  let req = await RoleApi.role.get(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function getRolePermission(roleId) {
  let req = await RoleApi.role.getRolePermission({
    roleId: roleId,
    clientId: store.state.login.user.clientId,
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
async function deleteRole(id) {
  try {
    let req = await RoleApi.role.delete(id)
    let message = 'Api error'
    if (req.ok && req.data) {
      return Promise.resolve(true)
    }
    else {
      message = getErrorMessage(req.error)
      return Promise.reject(message)
    }
  } catch (e) {
    return Promise.reject(e)
  } finally {
    //
  }
}

async function validateRoleNameByClient(role) {
  let message = 'Api error'
  let req = await RoleApi.role.validateRoleName(
    role.id,
    role.clientId,
    role.name
  )
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function fetchRolesByClientId(clientId) {
  let message = 'Api error'
  let req = await RoleApi.role.list({
    page: 1,
    limit: -1,
    search: null,
    sort: 'name asc',
    clientId: clientId,

  })
  if (req.ok && req.data) {
    // return Promise.resolve(req.data.items.filter(roles => roles.clientId == clientId))
    return Promise.resolve(req.data.items)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function fetchRolesByClientIdChange(clientId) {
  let message = 'Api error'
  let req = await RoleApi.role.list({
    page: 1,
    limit: -1,
    search: null,
    sort: 'name asc',
    clientId: clientId,

  })
  if (req.ok && req.data) {
    // return Promise.resolve(req.data.items.filter(roles => roles.clientId == clientId))
    return Promise.resolve(req.data.items)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function addPermission(permission) {
  let message = 'Api error'
  debugger;
  let action = RoleApi.role.addPermission
  const req = await action(permission)
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}
export {
  addUpdateRole, getRoleById, deleteRole, validateRoleNameByClient
  , fetchRolesByClientId, fetchRolesByClientIdChange, getRolePermission, addPermission
}
