clear;clc;
robot = importrobot('DBRobot_v2_new.urdf');
robot.DataFormat = 'row'; 
theta = [0,0,0,0,0,0];
show(robot,theta);
% interactiveRigidBodyTree(robot);

endEffectorName = robot.BodyNames{end};
tform = getTransform(robot, theta, endEffectorName);

% robot = importrobot('DBRobot_v2.urdf');
% robot.DataFormat = 'row';
% config = homeConfiguration(robot); % 默认姿态
% tform = zeros(4,4,6);
% for i = 1:length(robot.BodyNames)
%     bodyName = robot.BodyNames{i};
%     body = robot.getBody(bodyName);
%     if isempty(body.Parent) || strcmp(body.Parent, robot.BaseName)
%         tform(:,:,i) = getTransform(robot, config, bodyName, robot.BaseName);
%     else
%         tform(:,:,i) = getTransform(robot, config, bodyName, body.Parent.Name);
%     end
% end

