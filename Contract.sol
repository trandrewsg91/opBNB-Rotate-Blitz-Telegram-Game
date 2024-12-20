// SPDX-License-Identifier: MIT
pragma solidity ^0.8.17;

contract RotateDashTelegramGameopBNB {
    // Mapping to track dash points for each player
    mapping(address => uint256) private playerDashPoints;

    // Events
    event DashPointsUpdated(address indexed player, uint256 totalDashPoints);
    event DashPointsReset(address indexed player);

    // Function to view a player's current dash points
    function getPlayerDashPoints(
        address player
    ) external view returns (uint256) {
        return playerDashPoints[player];
    }

    // Function to increment a player's dash points by 1
    function incrementDashPoints(address player) external {
        require(player != address(0), "Invalid player address");

        playerDashPoints[player] += 1;

        emit DashPointsUpdated(player, playerDashPoints[player]);
    }

    // Function to reset a player's dash points to zero
    function resetDashPoints(address player) external {
        require(player != address(0), "Invalid player address");

        playerDashPoints[player] = 0;

        emit DashPointsReset(player);
    }
}
