<template>
    <div>
        <a-menu style="width: 100%"
                :default-selected-keys="defaultSelectedKeys"
                :default-open-keys="defaultOpenKeys"
                :mode="mode"
                :theme="theme">
            <a-sub-menu key="0" title="Step 0">
                <a-menu-item key="page_what_is_OPC_tool" onclick="handleOnClick('page_what_is_OPC_tool');" title="What is the Office of the Privacy Commissioner of Canada’s privacy impact assessment e-Tool?">
                    What is the Office of the Privacy Commissioner of Canada’s privacy impact assessment e-Tool?
                </a-menu-item>
                <a-menu-item key="page_role_of_OPC_tool" onclick="handleOnClick('page_role_of_OPC_tool');" title="What is the OPC’s role in the privacy impact assessment process?">
                    What is the OPC’s role in the privacy impact assessment process?
                </a-menu-item>
                <a-menu-item key="page_best_practices" onclick="handleOnClick('page_best_practices');" title="PIA report writing best practices">
                    PIA report writing best practices
                </a-menu-item>
                <a-menu-item key="page_privacy_notice" onclick="handleOnClick('page_privacy_notice');" title="Privacy notice">
                    Privacy notice
                </a-menu-item>
                <a-menu-item key="page_definitions" onclick="handleOnClick('page_definitions');" title="Definitions">
                    Definitions
                </a-menu-item>
            </a-sub-menu>
            <a-sub-menu key="1" title="Step 1">
                <a-sub-menu key="1.1" title="Before you begin">
                    <a-menu-item key="page_before_begin_q_0_1" onclick="handleOnClick('page_before_begin_q_0_1');" title="Legal Authority">
                        Legal Authority
                    </a-menu-item>
                    <a-menu-item key="page_before_begin_q_0_2" onclick="handleOnClick('page_before_begin_q_0_2');" title="Relevant legistation and policies">
                        Relevant legistation and policies
                    </a-menu-item>
                </a-sub-menu>
                <a-sub-menu key="1.2" title="Determine if a PIA is required">
                    <a-menu-item key="page_step_1_a" onclick="handleOnClick('page_step_1_a');" title="The name of your program or activity">
                        The name of your program or activity
                    </a-menu-item>
                    <a-menu-item key="page_step_1_b" onclick="handleOnClick('page_step_1_b');" title="Major changes or program contracted out">
                        Major changes or program contracted out
                    </a-menu-item>
                    <a-menu-item key="page_step_1_q_1_5" onclick="handleOnClick('page_step_1_q_1_5');" title="Personal information">
                        Personal information
                    </a-menu-item>
                    <a-menu-item key="page_step_1_q_1_6" onclick="handleOnClick('page_step_1_q_1_6');" title="PIA may not be required">
                        PIA may not be required
                    </a-menu-item>
                    <a-menu-item key="page_step_1_q_1_7" onclick="handleOnClick('page_step_1_q_1_7');" title="Administrative uses">
                        Administrative uses
                    </a-menu-item>
                    <a-menu-item key="page_step_1_q_1_8" onclick="handleOnClick('page_step_1_q_1_8');" title="PIA may not be required">
                        PIA may not be required
                    </a-menu-item>
                    <a-menu-item key="page_step_1_q_1_9" onclick="handleOnClick('page_step_1_q_1_9');" title="Your PIA subject">
                        Your PIA subject
                    </a-menu-item>
                    <a-menu-item key="page_step_1_q_1_10" onclick="handleOnClick('page_step_1_q_1_10');" title="PIA may not be required">
                        PIA may not be required
                    </a-menu-item>
                </a-sub-menu>
            </a-sub-menu>
        </a-menu>
    </div>
</template>
<script>
    export default {
        data() {
            return {
                mode: 'inline',
                theme: 'light',
                defaultOpenKeys: ['0', '1'],
                defaultSelectedKeys: ['page_what_is_OPC_tool']
            };
        }
    };
</script>
