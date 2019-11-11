var _renderOrderTable = null;
layui.use(['laydate', 'laypage', 'layer', 'table', 'form', 'carousel', 'upload', 'element'], function () {
    var form = layui.form, //表单
        laydate = layui.laydate, //日期            
        laypage = layui.laypage, //分页
        layer = layui.layer, //弹层
        table = layui.table, //表格
        carousel = layui.carousel, //轮播
        upload = layui.upload, //上传
        element = layui.element; //元素操作   


    _renderOrderTable = function () {
        //日期范围 初始化
        laydate.render({
            elem: '#createDate',
            range: true
        });

        //执行一个 table 实例
        table.render({
            elem: '#demoTable',
            // height: 471, //
            height: 545,
            cellMinWidth: 80, //全局定义常规单元格的最小宽度，layui 2.2.1 新增
            url: 'http://localhost:39070/static/WebUI/Index/MockData/userData.json', //数据接口                
            page: true, //开启分页                
            cols: [
                [ //表头
                    { fixed: true, checkbox: true, unresize: true }, //开启复选框        
                    { field: 'sortCode', fixed: 'left', title: '序号', width: 80, align: 'center', sort: true, unresize: true },
                    { field: 'username', title: '用户名', align: 'center', unresize: true },
                    { field: 'experience', title: '积分', align: 'center', sort: true, unresize: true },
                    { field: 'classify', title: '职业', align: 'center', unresize: true },
                    // { field: 'sex', title: '性别', width: 85, templet: '#switchTpl', unresize: true },
                    // { field: 'lock', title: '是否锁定', width: 110, templet: '#checkboxTpl', unresize: true },
                    { fixed: 'right', title: '数据操作', align: 'center', toolbar: '#barDemo', unresize: true }
                ]
            ],
            id: 'testReload' //数据刷新
        });

        //监听工具条
        table.on('tool(demoTable)', function (obj) {
            var data = obj.data;
            if (obj.event === 'detail') {
                layer.msg('ID：' + data.id + ' 的查看操作');
            } else if (obj.event === 'del') {
                layer.confirm('真的删除行么', function (index) {
                    obj.del();
                    layer.close(index);
                    tableReload();
                });
            } else if (obj.event === 'edit') {
                layer.alert('编辑行：<br>' + JSON.stringify(data))
            }
        });
        //监听性别操作
        form.on('switch(sexDemo)', function (obj) {
            layer.tips(this.value + ' ' + this.name + '：' + obj.elem.checked, obj.othis);
        });

        function tableReload() {
            table.reload('testReload', {
                page: {
                    curr: 1 //重新从第 1 页开始
                }
            });
        }

        var $ = layui.$,
            active = {
                reload: function () {
                    var demoReloadId = $('#demoReloadId');
                    //执行重载
                    table.reload('testReload', {
                        page: {
                            curr: 1 //重新从第 1 页开始
                        },
                        where: {
                            key: {
                                id: demoReloadId.val()
                            }
                        }
                    });
                }
            };

        $(document).on('click',"#demoReloadQuery", function () {
            var type = $(this).data('type');
            active[type] ? active[type].call(this) : '';
        });
    }
})
_renderOrderTable();