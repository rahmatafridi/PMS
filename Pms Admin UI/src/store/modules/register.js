import ClientApi from "@/api/client.api"

const registerInitialState = () => ({
    client: null,
    postRegisterUrl: '/register',
})

const state = registerInitialState()

const mutations = {
    setClient(state, client) {
        state.client = client
    },
    RESET(state) {
        const newState = registerInitialState();
        Object.keys(newState).forEach(key => {
            state[key] = newState[key]
        });
    },
}

const getters = {
}

const actions = {
    async register({ commit, dispatch }, { addClient }) {
        //Add client
        let req = await ClientApi.client.add(addClient)
        if (req.ok && req.data) {
            commit('setClient', req.data)
            //Copy roles for the client
            const rolesCopied = await ClientApi.client.copyRoles(req.data.id)
            if (rolesCopied) {
                let roleIds = []
                //Get roles by client id
                dispatch('role/getrolesbyclientid', req.data.id, { root: true })
                    .then(roleResponse => {
                        for (var i = 0; i < roleResponse.length; i++) {
                            roleIds.push(roleResponse[i].id)
                        }
                    }).catch(e => {
                        console.log("Exception occured while getting roles by client id " + e)
                        return Promise.reject("Exception occured while getting roles by client id " + e)
                    })
                const addUser = {
                    DisplayName: req.data.name,
                    Email: req.data.email,
                    UserName: req.data.email,
                    ClientId: req.data.id,
                }
                //Add user
                dispatch('user/addUpdate', addUser, { root: true }).then(userResponse => {
                    if (userResponse != null && userResponse != undefined) {
                        const assignRolesToUser = {
                            UserId: userResponse.id,
                            RoleIds: roleIds
                        }
                        //assign role to user
                        dispatch('user/assignRolesToUser',assignRolesToUser,
                            { root: true }).catch(e => {
                                console.log("Exception occured while assigning roles to user " + e)
                                return Promise.reject("Exception occured while assigning roles to user " + e)
                            })
                    }
                }).catch(e => {
                    console.log("Exception occured while registerng user " + e)
                    return Promise.reject("Exception occured while registerng user " + e)
                })
                return Promise.resolve()
            }
            console.log("Exception occured while registerng user ")
            return Promise.reject("Unable to copy roles for the client")
        }
        let message = 'Api error'
        if (req.error && req.error.response && req.error.response.data)
            message = req.error.response.data.message
        return Promise.reject(message)
    }
}

export default {
    namespaced: true,
    state,
    getters,
    actions,
    mutations,
}