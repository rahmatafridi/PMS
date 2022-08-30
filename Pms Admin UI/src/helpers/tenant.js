import TenantApi from '@/api/tenant.api'
import { getErrorMessage } from '@/helpers/error'

async function addUpdateTenant(addUpdateTenant) {
  let action = addUpdateTenant.id > 0 ?
    TenantApi.tenant.update : TenantApi.tenant.add
  let message = 'Api error'
  let req = await action(addUpdateTenant)
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  } else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function getTenantById(id) {
  let req = await TenantApi.tenant.get(id)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}

async function deleteTenant(id) {
  try {
    let message = 'Api error'

    let req = await TenantApi.tenant.delete(id)
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

const tenantInitialState = () => ({
  isLoading: false,
  list: {
    page: 1,
    perPage: 10,
    totalCount: 0,
    sort: [],
    search: null,
    items: [],
  },
})

const state = tenantInitialState()


async function loadTenant() {
  let message = 'Api error'
  let req = await TenantApi.tenant.list({
    page: state.list.page,
    limit: state.list.perPage,
    search: state.list.search || null,
    sort:
      state.list.sort.map(s => `${s.name} ${s.direction}`).join(',') ||
      null,
  })
  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
}

export { addUpdateTenant, getTenantById, deleteTenant, loadTenant }
