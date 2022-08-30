<template>
  <div class="row">



    <div class="col-md-12">
      <div class="card">
        <div class="card-header">
          <div class="row">
            <div class="col-lg-6">
              <label class="title headersize">Permissions</label>
            </div>
            <div class="col-lg-6">
              <button type="button" style="float:right" v-on:click="submit" class="btn btn-fill btn-info">Submit</button>

            </div>
          </div>
          <hr />
        </div>
        <div class="card-content">
          <!--<v-jstree :data="data" show-checkbox multiple allow-batch whole-row @item-click="itemClick"></v-jstree>-->
          <!--<v-treeview selectable
    :items="items"></v-treeview>-->
          <!--<TWTree :tree="tree" ref="tree" class="tree"
     :default-attrs="{checkbox: {show: true}}"
    />-->
          <!--<v-treeview checkbox
                item-disabled="locked"
                :items="items"></v-treeview>-->
          <tree v-if="shwoTree"
                :data="permissions"
                :options="treeOptions" ref="tree"  @node:checked="onNodeSelected" v-model="selected">

          </tree>
        </div>

      </div>
      </div>

  </div>
</template>
<script>

 
  //import VJstree from 'vue-jstree'
  import { getRolePermission, addPermission} from '@/helpers/role'
  import TWTree from 'twtree'

  export default {
    components: {
      TWTree
    },

    data() {
      return {
        shwoTree: false,
        tree: [],
        selected: null,
        permissions: [],
        treeOptions: {
          minFetchDelay: 1000,
           
          checkbox: true,
          //propertyNames: {
          //  text: 'feature',
          //  //id: 'feature_id',
          //  state: { checked: true },
          //  children:'permissions'
          //     }
        },
        submitArr: [],
        roleId:''
       
      
      }
    },
    methods: {
      getRolePermission: getRolePermission,
      addPermission: addPermission,
      itemClick(node) {
        console.log(node.model.text + ' clicked !')
      },
      async getPermission(id) {
        var rootTree = [];


        await this.getRolePermission(id).then(res => {
          //this.permissions = res;
          res.forEach((value, index) => {
            
            var child = [];
            value.permissions.forEach((value, index) => {
              child.push({ permission: value.feature_Id, text: value.feature, state: { checked: value.is_checked } })
            })
            rootTree = {
              text: value.feature,
              feature_id: value.feature_Id,
              children: child
            };
          
            this.permissions.push(rootTree);
            child = [];
           });
          this.shwoTree = true;
        })
      },
      submit() {
        var root = this;
        var data = this.selected.checked;
        var pdata = this.selected.checked.map(e => e.parent);
        this.selected.checked.map(el => el.text)
        console.log(this.permissions);
        debugger;
        var parr = [];
        root.submitArr = [];
        data.forEach((value, index) => {

          if (pdata[index] != null) {
            var resultP = pdata[index].text;

            // var valObj = this.permissions.filter(function (elem) {
            this.permissions.forEach((value1, index) => {
              if (value1.text == resultP) {
                value1.children.filter(function (e) {
                  if (e.text == value.text) {
                    root.submitArr.push(root.roleId + ',' + value1.feature_id + ',' + e.permission)
                  }
                })
              }


            })
            //if (elem.text == resultP) {
            //  elem.permissions.filter(function (e) {
            //    if (e.feature == value.text) {
            //      root.submitArr.push(root.roleId +','+ elem.feature_Id + ',' + e.feature_Id)
            //    }
            //  })

            //}
            //    return elem.feature;
            // });

            parr.push(resultP);
            console.log(this.submitArr);
          }
          });
      
        if (root.submitArr != '[]') {
          this.addPermission(root.submitArr)
            .then(res => {
              if (res != null || res != undefined)

                this.$notify(
                  {
                    component: {
                      template: `<span> <b>Added</b> </br> Permission Added Successfully</span>`
                    },
                    icon: '',
                    horizontalAlign: 'right',
                    verticalAlign: 'top',
                    type: 'info'
                  })
             this.$router.push('/roles')
            })
            .catch(e => {
              this.errorMessage = e;
              swal({
                title: 'Warning',
                text: this.errorMessage,
                type: 'warning',
                confirmButtonClass: 'btn btn-success btn-fill',
                buttonsStyling: false
              })
            })
        }
       
      },
      onNodeSelected(node) {
        console.log(node.parent.text)


      },
    },
    mounted() {
      let id = this.$route.params.id;
      this.roleId = id;
      this.getPermission(this.roleId);
    }
    //},
    //async created() {
    //  this.getPermission();
    //}
  }

  function getTreeData() {
    return [
      {
        text: 'JS: The Right Way',

        // it makes node expanded
        state: { expanded: true },
        children: [
          { text: 'Getting Started', state: { checked: true } },
          { text: 'JavaScript Code Style', state: { selected: true } },
          {
            text: 'The Good Parts', children: [
              { text: 'OBJECT ORIENTED', state: { checked: true } },
              { text: 'ANONYMOUS FUNCTIONS', state: { checked: true } },
              { text: 'FUNCTIONS AS FIRST-CLASS OBJECTS', state: { checked: true } },
              { text: 'LOOSE TYPING', state: { checked: true } }
            ]
          },
          {
            text: 'Patterns', children: [
              {
                text: 'DESIGN PATTERNS', state: { expanded: true }, children: [
                  {
                    text: 'Creational Design Patterns', children: [
                      { text: 'Factory' },
                      { text: 'Prototype' },
                      { text: 'Mixin' },
                      { text: 'Singleton' }
                    ]
                  },
                  { text: 'Structural Design Patterns' }
                ]
              },
              {
                text: 'MV* PATTERNS', cildren: [
                  { text: 'MVC Pattern' },
                  { text: 'MVP Pattern' },
                  { text: 'MVVM Pattern' }
                ]
              }
            ]
          }
        ]
      }
    ]
  }
</script>
