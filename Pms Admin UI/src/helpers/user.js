import UserApi from '@/api/user.api'
import { getErrorMessage } from '@/helpers/error'
import store from '@/store/index'

const userInitialState = () => ({
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

const state = userInitialState()

async function addUpdateUser(addUpdateUser) {
    debugger;

    let message = 'Api error'
    let action = addUpdateUser.id > 0 ? UserApi.user.update : UserApi.user.add
    const req = await action(addUpdateUser)
    if (req.ok && req.data) {
        let ids = []
        ids.push(addUpdateUser.roleIds)
        //assign role to user
        const assignRolesToUser = {
            userId: req.data.id,
            roleIds: addUpdateUser.roleIds
        }
        await assignRolesToUser1(assignRolesToUser).then(res => {
            if (res)
                return Promise.resolve(true)
        }).catch(ex => {
            return Promise.reject(ex)
        })
        return Promise.resolve(req.data)
    }
    else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}

async function getUserById(id) {
    let req = await UserApi.user.get(id)
    let message = 'Api error'

    if (req.ok && req.data) {
        return Promise.resolve(req.data)
    }
    else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}
async function getUserForProfile() {
  
  var id2 = store.state.login.user.id;

  let req = await UserApi.user.get(id2)
  let message = 'Api error'

  if (req.ok && req.data) {
    return Promise.resolve(req.data)
  }
  else {
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  }
}
async function deleteUser(id) {
    try {
        let req = await UserApi.user.delete(id)
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

async function assignRolesToUser1(assignRolesToUser) {
    let result = await UserApi.user.assignrolestouser(assignRolesToUser)
    let message = 'Api error'
    if (result.ok && result.data) {
        return Promise.resolve(result.data)
    }
    else {
        message = getErrorMessage(result.error)
        return Promise.reject(message)
    }
}

async function validateUserName(user) {
    let message = 'Api error'
    let req = await UserApi.user.validateUsername(user.id, user.userName)
    if (req.ok && req.data) {
        return Promise.resolve(req.data)
    } else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}

async function validateUserEmail(user) {
    let message = 'Api error'
    let req = await UserApi.user.validateEmail(user.id, user.email)
    if (req.ok && req.data) {
        return Promise.resolve(req.data)
    } else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}

async function getRolesByUserId(id) {
    let req = await UserApi.user.getrolesbyuserid(id)
    let message = 'Api error'

    if (req.ok && req.data) {
        return Promise.resolve(req.data)
    }
    else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
    }
}
async function getUserByClientId(id) {
  let req = await UserApi.user.listUserByClient({
    clientId:id,
    page: state.list.page,
    limit: state.list.perPage,
    search: state.list.search || null,
    sort:
      state.list.sort.map(s => `${s.name} ${s.direction}`).join(',') ||
      null,
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

export {
    addUpdateUser, getUserById, deleteUser, assignRolesToUser1
  , validateUserName, validateUserEmail, getRolesByUserId, getUserByClientId, getUserForProfile
}
