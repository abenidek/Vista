﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Vista.Data.AppDbContext;

#nullable disable

namespace Vista.Migrations
{
    [DbContext(typeof(VistaDbContext))]
    partial class VistaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Vista.Data.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("category_name");

                    b.HasKey("CategoryId");

                    b.ToTable("categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            CategoryName = "Sports"
                        },
                        new
                        {
                            CategoryId = 2,
                            CategoryName = "Photography"
                        },
                        new
                        {
                            CategoryId = 3,
                            CategoryName = "Travel"
                        },
                        new
                        {
                            CategoryId = 4,
                            CategoryName = "Cooking"
                        },
                        new
                        {
                            CategoryId = 5,
                            CategoryName = "Movies"
                        },
                        new
                        {
                            CategoryId = 6,
                            CategoryName = "Music"
                        },
                        new
                        {
                            CategoryId = 7,
                            CategoryName = "Art"
                        },
                        new
                        {
                            CategoryId = 8,
                            CategoryName = "Reading"
                        },
                        new
                        {
                            CategoryId = 9,
                            CategoryName = "Writing"
                        },
                        new
                        {
                            CategoryId = 10,
                            CategoryName = "Dancing"
                        },
                        new
                        {
                            CategoryId = 11,
                            CategoryName = "Gaming"
                        });
                });

            modelBuilder.Entity("Vista.Data.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("comment_id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("Now()");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("VideoId")
                        .HasColumnType("uuid")
                        .HasColumnName("video_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VideoId");

                    b.ToTable("comments");
                });

            modelBuilder.Entity("Vista.Data.Models.DislikedVideo", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("VideoId")
                        .HasColumnType("uuid")
                        .HasColumnName("video_id");

                    b.HasKey("UserId", "VideoId");

                    b.HasIndex("VideoId");

                    b.ToTable("disliked_video");
                });

            modelBuilder.Entity("Vista.Data.Models.LikedVideo", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("VideoId")
                        .HasColumnType("uuid")
                        .HasColumnName("video_id");

                    b.HasKey("UserId", "VideoId");

                    b.HasIndex("VideoId");

                    b.ToTable("liked_video");
                });

            modelBuilder.Entity("Vista.Data.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Bio")
                        .HasColumnType("text")
                        .HasColumnName("bio");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<int>("FollowersCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("followers");

                    b.Property<int>("FollowingCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("following");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("ProfilePicUrl")
                        .HasColumnType("text")
                        .HasColumnName("profile_pic_url");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("Vista.Data.Models.UserFollower", b =>
                {
                    b.Property<Guid>("FollowedUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("followed_user_id");

                    b.Property<Guid>("FollowerUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("follower_user_id");

                    b.HasKey("FollowedUserId", "FollowerUserId");

                    b.HasIndex("FollowerUserId");

                    b.ToTable("followers");
                });

            modelBuilder.Entity("Vista.Data.Models.Video", b =>
                {
                    b.Property<Guid>("VideoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("video_id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    b.Property<int>("Dislikes")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("dislikes");

                    b.Property<int>("Likes")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("likes");

                    b.Property<string>("ThumbnailUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("video_thumbnail_url");

                    b.Property<DateTime>("UploadDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("upload_date")
                        .HasDefaultValueSql("Now()");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("VideoDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("video_description");

                    b.Property<string>("VideoLength")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("video_length");

                    b.Property<string>("VideoName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("video_name");

                    b.Property<string>("VideoUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("video_url");

                    b.Property<int>("Views")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("views");

                    b.HasKey("VideoId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("videos");
                });

            modelBuilder.Entity("Vista.Data.Models.WatchedVideo", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("VideoId")
                        .HasColumnType("uuid")
                        .HasColumnName("video_id");

                    b.Property<DateTime>("ViewedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("viewed_at")
                        .HasDefaultValueSql("Now()");

                    b.HasKey("UserId", "VideoId");

                    b.HasIndex("VideoId");

                    b.ToTable("watched_videos");
                });

            modelBuilder.Entity("Vista.Data.Models.Comment", b =>
                {
                    b.HasOne("Vista.Data.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vista.Data.Models.Video", "Video")
                        .WithMany("Comments")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Vista.Data.Models.DislikedVideo", b =>
                {
                    b.HasOne("Vista.Data.Models.User", "User")
                        .WithMany("DislikedVideos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vista.Data.Models.Video", "Video")
                        .WithMany("DislikedVideos")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Vista.Data.Models.LikedVideo", b =>
                {
                    b.HasOne("Vista.Data.Models.User", "User")
                        .WithMany("LikedVideos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vista.Data.Models.Video", "Video")
                        .WithMany("LikedVideos")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Vista.Data.Models.UserFollower", b =>
                {
                    b.HasOne("Vista.Data.Models.User", "FollowedUser")
                        .WithMany("Followers")
                        .HasForeignKey("FollowedUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vista.Data.Models.User", "FollowerUser")
                        .WithMany()
                        .HasForeignKey("FollowerUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FollowedUser");

                    b.Navigation("FollowerUser");
                });

            modelBuilder.Entity("Vista.Data.Models.Video", b =>
                {
                    b.HasOne("Vista.Data.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vista.Data.Models.User", "User")
                        .WithMany("Videos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Vista.Data.Models.WatchedVideo", b =>
                {
                    b.HasOne("Vista.Data.Models.User", "User")
                        .WithMany("WatchedVideos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vista.Data.Models.Video", "Video")
                        .WithMany("WatchedVideos")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Vista.Data.Models.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("DislikedVideos");

                    b.Navigation("Followers");

                    b.Navigation("LikedVideos");

                    b.Navigation("Videos");

                    b.Navigation("WatchedVideos");
                });

            modelBuilder.Entity("Vista.Data.Models.Video", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("DislikedVideos");

                    b.Navigation("LikedVideos");

                    b.Navigation("WatchedVideos");
                });
#pragma warning restore 612, 618
        }
    }
}
